import time
from locust import HttpUser, task, between

class ApiLoadTest(HttpUser):
    wait_time = between(1, 10)

    @task
    def get_account(self):
        self.client.get(url='account/6SjXMF6QFphad1QaNQbPzhc98I22')

    @task
    def get_attempt(self):
        self.client.get(url='attempt/D2vrZQSiHUQuNG2u2qMT')
    
    @task
    def get_question(self):
        self.client.get(url='question/5XpEOTq7yUD36MhiVwq8')

    @task
    def get_gym(self):
        self.client.get(url='gym/2AcLYXxjcwUH8M6pS4kZ')

    @task
    def get_assignment(self):
        self.client.get(url='assignment/LUpzwPCNSUvWLw9NFeNM')