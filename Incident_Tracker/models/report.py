from utils.classifier import detect_type
from datetime import datetime


class ReportGenerator:

    def generate_html(self, incidents):

        # ===== COUNTS =====
        total = len(incidents)
        critical = len([i for i in incidents if i.severity == "critical"])
        high = len([i for i in incidents if i.severity == "high"])

        # ===== TYPE COUNT =====
        type_count = {"network": 0, "security": 0, "app": 0, "general": 0}
        security_threats = 0

        for i in incidents:
            t = detect_type(i.title + " " + i.description)
            type_count[t] += 1
            if t == "security":
                security_threats += 1

        # ===== SEVERITY COUNT (FIXED) =====
        severity_count = {
            "critical": 0,
            "high": 0,
            "medium": 0,
            "low": 0
        }

        for i in incidents:
            severity_count[i.severity] += 1

        # ===== TEAM COUNT =====
        team_count = {}
        for i in incidents:
            team_count[i.assigned_team] = team_count.get(i.assigned_team, 0) + 1

        generated_time = datetime.now().strftime("%Y-%m-%d %H:%M")

        # ===== TABLE ROWS =====
        rows = ""
        for i in incidents:
            incident_type = detect_type(i.title + " " + i.description)

            rows += f"""
<tr>
<td>{i.id}</td>
<td>🔹 {i.title}</td>
<td><span class="badge badge-{i.severity}">{i.severity.upper()}</span></td>
<td>{incident_type}</td>
<td>{i.assigned_team}</td>
<td>{i.timestamp.strftime("%Y-%m-%d %H:%M")}</td>
<td class="ticket">
<span class="snow">SNOW</span>: {i.ticket_ids.get("snow","-")}<br>
<span class="jira">JIRA</span>: {i.ticket_ids.get("jira","-")}<br>
<span class="azure">AZURE</span>: {i.ticket_ids.get("azure","-")}
</td>
</tr>
"""

        # ===== TEAM PILLS =====
        team_html = ""
        for team, count in team_count.items():
            team_html += f'<span class="pill general">{team}: {count}</span>'

        # ===== HTML =====
        html = f"""<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Incident Dashboard</title>

<style>
body {{
    font-family: "Segoe UI", sans-serif;
    background: #eef1f5;
    margin: 0;
    font-size: 12px;
    color: #333;
}}

.header {{
    background: #233a5e;
    color: white;
    padding: 8px 16px;
    font-size: 13px;
    display: flex;
    justify-content: space-between;
}}

.container {{
    padding: 12px 16px;
}}

.block {{
    background: #f8f9fb;
    border: 1px solid #dcdfe3;
    padding: 10px;
    margin-bottom: 10px;
    border-radius: 4px;
}}

.summary {{
    display: flex;
    gap: 12px;
}}

.card {{
    background: #ffffff;
    border: 1px solid #dcdfe3;
    border-radius: 6px;
    padding: 10px;
    width: 120px;
    text-align: center;
}}

.card h3 {{
    margin: 0;
    font-size: 18px;
}}

.card p {{
    margin: 2px 0 0;
    font-size: 11px;
    color: #666;
}}

.card.red h3 {{ color: #d9534f; }}
.card.orange h3 {{ color: #f0ad4e; }}
.card.purple h3 {{ color: #7e57c2; }}

h4 {{
    margin: 4px 0 8px;
    font-size: 13px;
}}

.pill {{
    display: inline-block;
    padding: 3px 8px;
    border-radius: 10px;
    font-size: 11px;
    margin-right: 5px;
}}

.network {{ background: #e3f2fd; color: #1976d2; }}
.security {{ background: #fdecea; color: #c62828; }}
.app {{ background: #ede7f6; color: #5e35b1; }}
.general {{ background: #eceff1; color: #546e7a; }}

.critical {{ background: #f8d7da; color: #c62828; }}
.high {{ background: #ffe0b2; color: #ef6c00; }}
.medium {{ background: #fff3cd; color: #f9a825; }}
.low {{ background: #d4edda; color: #2e7d32; }}

table {{
    width: 100%;
    border-collapse: collapse;
}}

th {{
    background: #233a5e;
    color: white;
    padding: 6px;
}}

td {{
    padding: 6px;
    border-bottom: 1px solid #e0e0e0;
}}

tr:hover {{
    background: #f4f6f8;
}}

.badge {{
    padding: 2px 6px;
    border-radius: 8px;
    font-size: 10px;
    font-weight: bold;
}}

.badge-critical {{ background: #d9534f; color: white; }}
.badge-high {{ background: #f0ad4e; color: white; }}
.badge-medium {{ background: #f7c948; color: black; }}
.badge-low {{ background: #5cb85c; color: white; }}

.ticket span {{
    font-weight: 500;
}}

.snow {{ color: #2e7d32; }}
.jira {{ color: #1565c0; }}
.azure {{ color: #6a1b9a; }}

</style>
</head>

<body>

<div class="header">
    <div>IT Incident Auto-Triage Report</div>
    <p>Generated: {generated_time} | Total: {total}</p>
</div>

<div class="container">

<div class="block">
<h4>Summary</h4>
<div class="summary">
    <div class="card">
        <h3>{total}</h3>
        <p>Total Incidents</p>
    </div>
    <div class="card red">
        <h3>{critical}</h3>
        <p>Critical</p>
    </div>
    <div class="card orange">
        <h3>{high}</h3>
        <p>High</p>
    </div>
    <div class="card purple">
        <h3>{security_threats}</h3>
        <p>Security Threats</p>
    </div>
</div>
</div>

<div class="block">
<h4>Breakdown by Type</h4>
<span class="pill network">network: {type_count['network']}</span>
<span class="pill security">security: {type_count['security']}</span>
<span class="pill app">app: {type_count['app']}</span>
<span class="pill general">general: {type_count['general']}</span>
</div>

<div class="block">
<h4>Breakdown by Severity</h4>
<span class="pill critical">critical: {severity_count['critical']}</span>
<span class="pill high">high: {severity_count['high']}</span>
<span class="pill medium">medium: {severity_count['medium']}</span>
<span class="pill low">low: {severity_count['low']}</span>
</div>

<div class="block">
<h4>Breakdown by Team</h4>
{team_html}
</div>

<div class="block">
<h4>Incident Detail</h4>

<table>
<tr>
<th>ID</th>
<th>Title</th>
<th>Severity</th>
<th>Type</th>
<th>Team</th>
<th>Timestamp</th>
<th>Tickets</th>
</tr>

{rows}

</table>
</div>

</div>

</body>
</html>
"""

        with open("output/report.html", "w", encoding="utf-8") as f:
            f.write(html)