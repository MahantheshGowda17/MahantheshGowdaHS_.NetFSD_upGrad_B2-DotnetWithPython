import re

network_pattern = re.compile(r"(tcp|udp|icmp|vlan|switch|ip|dns)", re.I)
security_pattern = re.compile(r"(breach|malware|phishing|ransomware|unauthorized)", re.I)
app_pattern = re.compile(r"(error|exception|http|nullpointer|timeout|api)", re.I)

critical_pattern = re.compile(r"(outage|down|breach|ransomware|critical)", re.I)
high_pattern = re.compile(r"(timeout|unreachable|failed)", re.I)
medium_pattern = re.compile(r"(slow|warning|intermittent)", re.I)


def detect_type(text):
    if network_pattern.search(text):
        return "network"
    elif security_pattern.search(text):
        return "security"
    elif app_pattern.search(text):
        return "app"
    return "general"


def detect_severity(text):
    if critical_pattern.search(text):
        return "critical"
    elif high_pattern.search(text):
        return "high"
    elif medium_pattern.search(text):
        return "medium"
    return "low"