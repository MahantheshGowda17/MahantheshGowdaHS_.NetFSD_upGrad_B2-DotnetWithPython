import requests
import base64
from config import MOCK_API, JIRA
from utils.decorators import log_call, retry


@log_call
@retry(times=3)
def create_ticket(incident):

    if MOCK_API:
        print("MOCK MODE")
        return "MOCK-JIRA"

    url = JIRA["domain"] + "/rest/api/3/issue"

    auth = base64.b64encode(
        f"{JIRA['email']}:{JIRA['api_token']}".encode()
    ).decode()

    headers = {
        "Authorization": f"Basic {auth}",
        "Content-Type": "application/json"
    }

    payload = {
        "fields": {
            "project": {
                "key": "SUP"   # CHANGE THIS
            },
            "summary": incident.title,
            "description": {
                "type": "doc",
                "version": 1,
                "content": [
                    {
                        "type": "paragraph",
                        "content": [
                            {"type": "text", "text": incident.description}
                        ]
                    }
                ]
            },
            "issuetype": {
                "name": "Task"
            }
        }
    }

    response = requests.post(url, headers=headers, json=payload)

    print("Jira Response:", response.status_code, response.text)

    response.raise_for_status()

    data = response.json()
    ticket_id = data["key"]

    incident.ticket_ids["jira"] = ticket_id
    return ticket_id