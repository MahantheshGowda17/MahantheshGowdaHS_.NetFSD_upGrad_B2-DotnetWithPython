import json
from models.incident import Incident
from utils.classifier import detect_type, detect_severity
from services import jira, servicenow, azure_boards
from models.report import ReportGenerator


def load_data():
    with open("data/incidents.json") as f:
        return json.load(f)


def process():
    data = load_data()
    incidents = []

    for inc in data:
        incident = Incident(
            inc["id"],
            inc["title"],
            inc["description"],
            inc["reported_by"],
            inc["timestamp"],
            inc["assigned_team"]
        )

        text = incident.title + " " + incident.description

        incident.set_severity(detect_severity(text))
        inc_type = detect_type(text)

        print(f"{incident.id} → {inc_type} | {incident.severity}")

       
        jira.create_ticket(incident)
        servicenow.create_ticket(incident)
        azure_boards.create_ticket(incident)

        incidents.append(incident)

    
    report = ReportGenerator()
    report.generate_html(incidents)

    print("\n Report generated: output/report.html")


if __name__ == "__main__":
    process()