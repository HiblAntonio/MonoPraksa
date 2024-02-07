-- Insert data into Customer table
INSERT INTO "Customer" ("Firstname", "Lastname", "Dob") 
VALUES 
    ('John', 'Doe', '1990-01-01 00:00:00'),
    ('Jane', 'Smith', '1985-05-15 00:00:00'),
    ('Michael', 'Johnson', '1988-07-20 00:00:00'),
    ('Emily', 'Brown', '1992-09-10 00:00:00'),
    ('David', 'Wilson', '1979-03-25 00:00:00');

-- Insert data into Ticket table
INSERT INTO "Ticket" ("CustomerId", "Price", "SeatNumber") 
VALUES 
    (1, 50.00, 101),
    (2, 45.00, 102),
    (3, 55.00, 103),
    (4, 60.00, 104),
    (5, 40.00, 105);

-- Insert data into Theater table
INSERT INTO "Theater" ("Address", "Capacity", "Size") 
VALUES 
    ('123 Main St', 200, 1500.00),
    ('456 Broadway', 300, 2000.00),
    ('789 Elm St', 250, 1800.00),
    ('101 Pine St', 400, 2500.00),
    ('202 Oak St', 350, 2200.00);

-- Insert data into Actor table
INSERT INTO "Actor" ("FirstName", "LastName", "Dob", "Salary") 
VALUES 
    ('Tom', 'Hanks', '1956-07-09 00:00:00', 1000000.00),
    ('Meryl', 'Streep', '1949-06-22 00:00:00', 1500000.00),
    ('Leonardo', 'DiCaprio', '1974-11-11 00:00:00', 2000000.00),
    ('Angelina', 'Jolie', '1975-06-04 00:00:00', 1800000.00),
    ('Brad', 'Pitt', '1963-12-18 00:00:00', 1900000.00);

-- Insert data into Genre table
INSERT INTO "Genre" ("Type") 
VALUES 
    ('Drama'),
    ('Comedy'),
    ('Action'),
    ('Romance'),
    ('Thriller');

-- Insert data into Play table
INSERT INTO "Play" ("GenreId", "Title") 
VALUES 
    (1, 'Hamlet'),
    (2, 'The Importance of Being Earnest'),
    (3, 'Inception'),
    (4, 'Titanic'),
    (5, 'The Dark Knight');

-- Insert data into Performance table
INSERT INTO "Performance" ("ActorId", "TheaterId", "PlayId", "Time", "Date") 
VALUES 
    (1, 1, 1, '2024-02-06 19:00:00', '2024-02-06'),
    (2, 2, 2, '2024-02-07 19:30:00', '2024-02-07'),
    (3, 3, 3, '2024-02-08 20:00:00', '2024-02-08'),
    (4, 4, 4, '2024-02-09 18:45:00', '2024-02-09'),
    (5, 5, 5, '2024-02-10 21:15:00', '2024-02-10');

CREATE TABLE "Example"(
	"Id" INT PRIMARY KEY,
	"Name" VARCHAR(50),
	"Lastname" VARCHAR(50)
);

ALTER TABLE "Example"
ADD COLUMN "ColumnName" VARCHAR(50);

ALTER TABLE "Example"
DROP COLUMN "Name";

DROP TABLE "Example";

SELECT *
FROM "Play"
LEFT JOIN "Actor" ON "Play"."Id" = "Actor"."Id";

SELECT *
FROM "Play"
RIGHT JOIN "Actor" ON "Play"."Id" = "Actor"."Id";