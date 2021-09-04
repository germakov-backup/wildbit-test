#Overview
Wildbit [test task](https://www.notion.so/Wildbit-Sr-Software-Engineer-Asynchronous-technical-assignment-0376cd60c3434423ac0046f3891404bd).
Implements resilient email sending pipeline supporting horizontal scalability of individual processing steps.

## How To Run

- Run Environment dependencies:
  
  with docker running Linux containers
  ```
  EmailSender.Environment>docker-compose build
  EmailSender.Environment>docker-compose up
  ```
  
  wait for initial base image pull - it might take several minutes 
  
- Run applications
   * EmailSender.Web - web server and host for basic sending pipeline for transactional messages
   * EmailSender.PromoMailSender.Host - dedicated sender for promotional emails
   * EmailSender.PriorityMailSender.Host - additional worker for prioritized messages sending

Database connection(if needed for manal testing) - host: localhost, db: messages, user/pass: postgres/postgres
    
##Conventions and short-cuts

* `nobody@nowhere.com` - considered blocked address for both `to` and `from` fields.
In `from` field this value leads to synchronous email rejection from API endpoint.
In `to` field this value leads to asynchronous email rejection - message send to error queue and marked as failed
  
* `promo` value in `messageStream` field branch processing pipeline to make sending from dedicated worker
* message priority calculation is caclculated randomly for demonstration purposes
* priority queue throughput increase is done by introducing additional dedicated consumer(for demo). rabbitMQ support priority queues, but for demonstration I used more generic pattern
* pipeline is configured with config files for simplicity.
* mail sending is emulated with writing to the logs
* part of code after sending emails and transaction commit with status change has small chance of potential resilience issues - if database goes down. 
no special handling was added here - as one of options, send this message to additional introspection queue.
*one of the corners cut for speed is not splitting message payload and metadata (`IMessageMetadata` implemented directly)

##Design ideas
* Used table row gateway for data access(just because data model is tiny, and also wanted to try out dapper)
* Web layer is pretty standard
* Web server processing part is kept as thin as possible for resilience - with write-ahead approach, messages are persisted to data store and eventually processed even in case of transient infrastructure failure.
* persistent middleware is used with ACK after messages processing to utilize built-in retry mechanism. Messages supposed to be deduplicated in mails sender filter
* Business logic is implemented in `EmailSender.PipelineHandlers.Handlers`
message handlers supposed to be pipeline agnostic and dedicated to single responsibility.
Pipeline-specific information(channel addresses) is injected via configuration.
  for example, `EmailSenderFilter` handles 3 queues from 3 different processes.
  
* Service declarations(implementations of `HandlerServiceBase`) supposed to be declarative configuration and host worker process registration adapters.


###Tags
asp.net core, docker, rabbit-mq, dapper, postgres
