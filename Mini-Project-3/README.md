# Name: Mahanthesh Gowda H S
# Batch: B2( uGE_Dotnet FSD with Python)
# Project: IT Incident Auto-Triage System


### Install Dependencies

```bash
pip install requests
```

---

###  Configure APIs

Update `config.py`:

```python
MOCK_API = False

SERVICENOW = {
    "instance_url": "https://your-instance.service-now.com",
    "username": "admin",
    "password": "your_password"
}

JIRA = {
    "domain": "https://your-domain.atlassian.net",
    "email": "your-email",
    "api_token": "your-token",
    "project_key": "SUP"
}

AZURE = {
    "organization": "your-org",
    "project": "your-project",
    "pat": "your-personal-access-token"
}
```

---

## Run the Project

```bash
py main.py
```

---

##  Output

After running:

*  Tickets created in:

  * Jira
  * ServiceNow
  * Azure Boards

* HTML Report generated:

```
output/report.html
```

