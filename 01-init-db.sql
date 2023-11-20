

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DROP TABLE IF EXISTS public.user;

CREATE TABLE IF NOT EXISTS public."user"
(
    id bigint primary key generated always as identity,
    email text COLLATE pg_catalog."default" NOT NULL,
    password_hash text COLLATE pg_catalog."default" NOT NULL,    
    CONSTRAINT email_unique UNIQUE (email)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;


DROP TABLE IF EXISTS public.prescription;

CREATE TABLE IF NOT EXISTS public.prescription
(
    id uuid DEFAULT uuid_generate_v4() NOT NULL,
    user_id bigint NOT NULL,
    drug text COLLATE pg_catalog."default" NOT NULL,
    dosage text COLLATE pg_catalog."default" NOT NULL,
	notes text COLLATE pg_catalog."default" NOT NULL,
    created_by bigint NOT NULL,
    created_at timestamp without time zone NOT NULL,
    modified_by bigint NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    CONSTRAINT prescription_pkey PRIMARY KEY (id),
    CONSTRAINT prescription_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public."user" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
    