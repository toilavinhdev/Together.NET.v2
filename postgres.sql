-- dbs: \l
-- use: \c Together.NET.v2
-- roles: \du+
-- sudo -u postgres createuser --interactive
-- \password admin
-- sudo -u admin createdb Together.NET.v2

CREATE USER roleName WITH PASSWORD 'password';
ALTER USER roleName WITH SUPERUSER;
GRANT ALL PRIVILEGES ON DATABASE "Together.NET.v2" TO roleName;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO roleName;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO roleName;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO roleName;

CREATE DATABASE "Together.NET.v2";

INSERT INTO "Forums" ("Id","Name","CreatedAt","ModifiedAt","CreatedById","ModifiedById") VALUES
('12d5b5d9-6a0d-4d4c-949a-3abb2197cc81','Khu vực điều hành',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('a6f89c64-1b97-401d-ad86-c820af1dedf6','Học tập & Sự nghiệp',now() + INTERVAL '1 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL),
('ee48f43f-a3d0-43b0-9731-fc74ab8d7b95','Sản phẩm công nghệ',now() + INTERVAL '2 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL),
('d38f29bf-60cf-4d83-ae13-508dfbd1a37b','Khác',now() + INTERVAL '3 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL);

INSERT INTO "Topics" ("Id","ForumId","Name","Description","CreatedAt","ModifiedAt","CreatedById","ModifiedById") VALUES
('e07f8af3-3ac5-4107-9fa7-0b8c33996a0c','12d5b5d9-6a0d-4d4c-949a-3abb2197cc81','Thông Báo Từ BQT','Các thông báo mới từ ban quản trị',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('96b5cbca-feb3-4e35-bfc5-035b088a3a25','a6f89c64-1b97-401d-ad86-c820af1dedf6','Tuyển dụng & Tìm việc','Nơi đăng tin tuyển dụng và tìm việc',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('f6915a81-b095-45c7-b314-9b8a580bb03d','a6f89c64-1b97-401d-ad86-c820af1dedf6','Lập trình - IT','Fix bugs never give up',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('db66572c-e11d-4ce2-9298-5d1628ffd331','a6f89c64-1b97-401d-ad86-c820af1dedf6','Ngoại ngữ','Góc ngoại ngữ',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('9b09a60f-dd7e-47dc-a4d5-4442973b2854','ee48f43f-a3d0-43b0-9731-fc74ab8d7b95','IOS','Hội những người dùng IOS',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('92c0f21e-1524-4e17-a02c-8edacb91f168','ee48f43f-a3d0-43b0-9731-fc74ab8d7b95','Android','Hội những người yêu thích Android',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('1ede6508-35e2-4d60-95ad-62c89eeb5f8d','ee48f43f-a3d0-43b0-9731-fc74ab8d7b95','Chụp ảnh & Quay phim','Giao lưu & Chia sẻ kinh nghiệm',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('6d5c5faa-5a5d-4c46-972e-8ee41d17512c','d38f29bf-60cf-4d83-ae13-508dfbd1a37b','Log bugs','Nơi tiếp nhận & xử lý bugs của Together.NET',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('faaa0493-2fed-4995-98be-146ae7e71a4d','d38f29bf-60cf-4d83-ae13-508dfbd1a37b','Nhận xét sản phẩm','Tất cả nhận xét về project Together.NET',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL);

INSERT INTO "Prefixes" ("Id","Name","Foreground","Background","CreatedAt","ModifiedAt","CreatedById","ModifiedById") VALUES
('6eba210b-e4c2-4642-a0dc-7b9d9d388807','Thông báo','#FFFFFF','#E20000',now(),NULL,'00000000-0000-0000-0000-000000000000',NULL),
('cd7e378a-ae61-4bcb-96e9-ffb7c14ec7da','Kiến thức','#FFFFFF','#008000',now() + INTERVAL '1 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL),
('346b02c7-8a9a-41b5-b6cd-c4b68b23d9af','Thảo luận','#000000','#FFCB00',now() + INTERVAL '2 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL),
('929b6de0-16e2-449b-a660-ad72a63505b0','Chia sẻ','#FFFFFF','#4338CA',now() + INTERVAL '3 minutes',NULL,'00000000-0000-0000-0000-000000000000',NULL);
