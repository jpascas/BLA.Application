
DROP TABLE IF EXISTS public.user;

CREATE TABLE IF NOT EXISTS public."user"
(
    id bigint primary key generated always as identity,
    email text COLLATE pg_catalog."default" NOT NULL,
    password text COLLATE pg_catalog."default" NOT NULL,    
    CONSTRAINT email_unique UNIQUE (email)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

    