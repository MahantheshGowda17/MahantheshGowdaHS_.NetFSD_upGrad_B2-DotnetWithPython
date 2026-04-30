import requests
import base64
from config import MOCK_API, AZURE
from utils.decorators import log_call, retry


@log_call
@retry(times=3)
def create_ticket(incident):

    if MOCK_API:
        ticket_id = "MOCK-AZURE"
        print(f"[MOCK] Azure Ticket Created: {ticket_id}")
        incident.ticket_ids["azure"] = ticket_id
        return ticket_id

    url = f"https://dev.azure.com/{AZURE['organization']}/{AZURE['project']}/_apis/wit/workitems/$Task?api-version=7.0"

    auth = base64.b64encode(f":{AZURE['pat']}".encode()).decode()

    headers = {
        "Content-Type": "application/json-patch+json",
        "Authorization": f"Basic {auth}"
    }

    payload = [
        {"op": "add", "path": "/fields/System.Title", "value": incident.title},
        {"op": "add", "path": "/fields/System.Description", "value": incident.description}
    ]

    response = requests.post(url, headers=headers, json=payload)

    print("Azure Response:", response.status_code, response.text)

    response.raise_for_status()

    data = response.json()
    ticket_id = data["id"]

    incident.ticket_ids["azure"] = ticket_id
    return ticket_id