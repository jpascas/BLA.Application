
-------
--USERS
-------

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
CREATE OR REPLACE FUNCTION public.sp_getbyid_user (p_user_id bigint) RETURNS TABLE (
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
CREATE OR REPLACE FUNCTION public.sp_getbyemail_user (p_email  text) RETURNS TABLE (
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

---------------
--PRESCRIPTIONS
---------------

DROP FUNCTION IF EXISTS public.sp_getbyid_prescription;

-- select * from sp_getbyid_prescription('9db22a4d-be7b-4793-b9f6-6d0ed0b761ba')

CREATE OR REPLACE FUNCTION public.sp_getbyid_prescription(p_id uuid) 
RETURNS TABLE (
          id uuid,
    	  user_id bigint,
    	  drug text,
    	  dosage text,
		  notes text,
    	  created_by bigint,
    	  created_at timestamp without time zone,
    	  modified_by bigint,
    	  modified_at timestamp without time zone
) 
    AS '
BEGIN
 RETURN QUERY SELECT 
 		  p.id,
    	  p.user_id,
    	  p.drug,
    	  p.dosage,
		  p.notes,
    	  p.created_by,
    	  p.created_at,
    	  p.modified_by,
    	  p.modified_at
	FROM public.prescription p
  WHERE p.id = p_id;
END;
' LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS public.sp_getbyuserid_prescription;

-- select * from sp_getbyuserid_prescription(1)

CREATE OR REPLACE FUNCTION public.sp_getbyuserid_prescription(p_user_id bigint) 
RETURNS TABLE (
          id uuid,
    	  user_id bigint,
    	  drug text,
    	  dosage text,
		  notes text,
    	  created_by bigint,
    	  created_at timestamp without time zone,
    	  modified_by bigint,
    	  modified_at timestamp without time zone
) 
    AS '
BEGIN
 RETURN QUERY SELECT 
 		  p.id,
    	  p.user_id,
    	  p.drug,
    	  p.dosage,
		  p.notes,
    	  p.created_by,
    	  p.created_at,
    	  p.modified_by,
    	  p.modified_at
	FROM public.prescription p
  WHERE p.user_id = p_user_id;
END;
' LANGUAGE plpgsql;



DROP FUNCTION IF EXISTS public.sp_insert_prescription;

--select * from public.sp_insert_prescription(1,'drug','dosage','notes',1)

CREATE OR REPLACE FUNCTION public.sp_insert_prescription(
	      p_user_id bigint,
    	  p_drug text,
    	  p_dosage text,
		  p_notes text,
    	  p_created_by bigint) 
RETURNS TABLE (
          id uuid,
    	  user_id bigint,
    	  drug text,
    	  dosage text,
		  notes text,
    	  created_by bigint,
    	  created_at timestamp without time zone,
    	  modified_by bigint,
    	  modified_at timestamp without time zone
) 
    AS '
DECLARE 
	f_current_time timestamp without time zone := sp_get_current_utc_date();	
BEGIN
  RETURN QUERY INSERT INTO public.prescription (	    		  
    	  user_id,
    	  drug,
    	  dosage,
		  notes,
    	  created_by,
    	  created_at,
    	  modified_by,
    	  modified_at
  )
  VALUES (	    
		  p_user_id,
    	  p_drug,
    	  p_dosage,
		  p_notes,
    	  p_created_by,
    	  f_current_time,
    	  p_created_by,
    	  f_current_time
  )
  RETURNING *
  ;
END;
' 
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS public.sp_update_prescription;

--select * from public.sp_update_prescription('9db22a4d-be7b-4793-b9f6-6d0ed0b761ba','dosage modified','notes modified',1)

CREATE OR REPLACE FUNCTION public.sp_update_prescription(
		  p_id uuid,
    	  p_dosage text,
		  p_notes text,
    	  p_modified_by bigint) 
RETURNS TABLE (
          id uuid,
    	  user_id bigint,
    	  drug text,
    	  dosage text,
		  notes text,
    	  created_by bigint,
    	  created_at timestamp without time zone,
    	  modified_by bigint,
    	  modified_at timestamp without time zone
) 
    AS '
DECLARE 
	f_current_time timestamp without time zone := sp_get_current_utc_date();	
BEGIN
  RETURN QUERY UPDATE public.prescription p SET	    		      	  
    	  dosage = p_dosage,   
		  notes = p_notes,
    	  modified_by = p_modified_by, 
    	  modified_at = f_current_time
		WHERE p.id = p_id
  RETURNING *
  ;
END;
' 
LANGUAGE plpgsql;


DROP FUNCTION IF EXISTS public.sp_deletebyid_prescription;

-- select * from sp_deletebyid_prescription('5486fead-f610-4dcd-be3f-0c01e26512a4')

CREATE FUNCTION public.sp_deletebyid_prescription(p_id uuid) 
RETURNS void
    AS '
BEGIN
	 DELETE FROM public.prescription	 
  	 WHERE id = p_id;
END;
' LANGUAGE plpgsql;





