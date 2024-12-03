select * from pg_indexes
where schemaname = 'public';


select * from pg_sequences
where schemaname = 'public';


select * FROM information_schema.table_constraints
WHERE table_schema = 'public';
