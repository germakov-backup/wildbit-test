CREATE TYPE MessageStatus AS ENUM ('Pending', 'Processing', 'Sent', 'Failed', 'Bounced');

CREATE TABLE Messages (
    id SERIAL primary key not null,
    sender varchar(250) not null,
    destination varchar(250) not null,
    subject varchar(1024),
    message_stream varchar(250) not null,
    text_body text,
    html_body text,
    status MessageStatus default 'Pending',
    created TIMESTAMPTZ DEFAULT Now()
);

create index idx_messages_status on Messages(status);
