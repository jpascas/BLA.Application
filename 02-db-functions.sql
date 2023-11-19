

DROP FUNCTION IF EXISTS public.sp_get_current_utc_date();
DROP FUNCTION IF EXISTS public.sp_getbyid_user(bigint);
DROP FUNCTION IF EXISTS public.sp_getbyemail_user(text);
DROP FUNCTION IF EXISTS public.sp_insert_user;


CREATE OR REPLACE FUNCTION sp_get_current_utc_date()
    RETURNS timestamp without time zone
AS '
BEGIN
  return now() at time zone ''utc'';
END;
' LANGUAGE plpgsql;

-- Definition for function sp_getbyid_user
CREATE FUNCTION public.sp_getbyid_user (p_user_id bigint) RETURNS TABLE (
        id bigint,
        email text,
        password_hash text
) 
    AS '
BEGIN
 RETURN QUERY SELECT u.id, u.email, u.password_hash
	FROM public.user u
  WHERE u.id = p_user_id;
END;
' LANGUAGE plpgsql;

-- Definition for function sp_getbyemail_user
CREATE FUNCTION public.sp_getbyemail_user (p_email  text) RETURNS TABLE (
        id bigint,
        email text,
        password_hash text
) 
    AS '
BEGIN
 RETURN QUERY SELECT u.id, u.email, u.password_hash
	FROM public.user u
  WHERE u.email ilike p_email;
END;
' LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.sp_insert_user(
	p_email text,
	p_password_hash text) RETURNS TABLE (
        id bigint,
        email text,
        password_hash text
) 
    AS '
DECLARE 
	f_current_time timestamp without time zone := sp_get_current_utc_date();	
BEGIN
  RETURN QUERY INSERT INTO public.user (	    
	email,        
	password_hash
  )
  VALUES (	    
	p_email,	
	p_password_hash
  )
  RETURNING *
  ;
END;
' 
LANGUAGE plpgsql;