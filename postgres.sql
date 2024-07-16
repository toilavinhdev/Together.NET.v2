-- dbs: \l
-- use: \c Together.NET.v2
-- roles: \du+

-- sudo -u postgres createuser --interactive
CREATE USER user WITH PASSWORD 'password';
-- \password admin
ALTER USER user WITH SUPERUSER;

-- sudo -u admin createdb Together.NET.v2
CREATE DATABASE "Together.NET.v2";

GRANT ALL PRIVILEGES ON DATABASE "Together.NET.v2" TO user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO user;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO user;

UPDATE "Roles"
SET "Claims" = '{User:Me,User:Get,User:UpdateProfile,User:UpdatePassword,Forum:View,Prefix:View,Post:View,Post:Create,Reply:Create}'
WHERE "Name" = 'Member';
-- WHERE "Id" = 'f96ed733-41c7-4530-985a-c97092e17fe3';

