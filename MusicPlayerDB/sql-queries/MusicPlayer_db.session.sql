create or replace function trigger_before_insert_users() 
returns trigger as $$
begin

    return new;
end;
$$ language plpgsql
