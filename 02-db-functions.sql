


DROP FUNCTION IF EXISTS public.sp_getbyid_user(uuid);
DROP FUNCTION IF EXISTS public.sp_getbyemail_user(text);

-- Definition for function sp_getbyid_user
CREATE FUNCTION public.sp_getbyid_user (p_user_id uuid) RETURNS TABLE (
        id bigint,
        email text,
        password text
) 
    AS '
BEGIN
 RETURN QUERY SELECT id, email, password
	FROM public.user
  WHERE id = p_user_id;
END;
' LANGUAGE plpgsql;

-- Definition for function sp_getbyemail_user
CREATE FUNCTION public.sp_getbyemail_user (p_email  text) RETURNS TABLE (
        id bigint,
        email text,
        password text
) 
    AS '
BEGIN
 RETURN QUERY SELECT id, email, password
	FROM public.user
  WHERE id = p_email;
END;
' LANGUAGE plpgsql;
