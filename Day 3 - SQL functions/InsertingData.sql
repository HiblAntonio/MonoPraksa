-- Customer Table Examples
INSERT INTO "Customer" ("Id", "Firstname", "Lastname", "Dob")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc964ff', 'John', 'Doe', '1990-01-01 00:00:00'),
    ('6f9619ff-8b86-d011-b42d-00c04fc965ff', 'Jane', 'Smith', '1985-05-15 00:00:00'),
    ('6f9619ff-8b86-d011-b42d-00c04fc966ff', 'Michael', 'Johnson', '1988-07-20 00:00:00'),
    ('6f9619ff-8b86-d011-b42d-00c04fc967ff', 'Emily', 'Brown', '1992-09-10 00:00:00'),
    ('6f9619ff-8b86-d011-b42d-00c04fc968ff', 'David', 'Wilson', '1979-03-25 00:00:00');

-- Ticket Table Examples
INSERT INTO "Ticket" ("Id", "CustomerId", "Price", "SeatNumber")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc974ff', '6f9619ff-8b86-d011-b42d-00c04fc964ff' , 50.00, 101),
    ('6f9619ff-8b86-d011-b42d-00c04fc975ff', '6f9619ff-8b86-d011-b42d-00c04fc964ff' , 45.00, 102),
    ('6f9619ff-8b86-d011-b42d-00c04fc976ff', '6f9619ff-8b86-d011-b42d-00c04fc964ff', 55.00, 103),
    ('6f9619ff-8b86-d011-b42d-00c04fc977ff', '6f9619ff-8b86-d011-b42d-00c04fc964ff', 60.00, 104),
    ('6f9619ff-8b86-d011-b42d-00c04fc978ff', '6f9619ff-8b86-d011-b42d-00c04fc964ff', 40.00, 105);

-- Theater Table Examples
INSERT INTO "Theater" ("Id", "Address", "Capacity", "Size")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc984ff', '123 Main St', 200, 1500.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc985ff', '456 Broadway', 300, 2000.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc986ff', '789 Elm St', 250, 1800.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc987ff', '101 Pine St', 400, 2500.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc988ff', '202 Oak St', 350, 2200.00);

-- Actor Table Examples
INSERT INTO "Actor" ("Id", "FirstName", "LastName", "Dob", "Salary")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc994ff', 'Tom', 'Hanks', '1956-07-09 00:00:00', 1000000.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc995ff', 'Meryl', 'Streep', '1949-06-22 00:00:00', 1500000.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc996ff', 'Leonardo', 'DiCaprio', '1974-11-11 00:00:00', 2000000.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc997ff', 'Angelina', 'Jolie', '1975-06-04 00:00:00', 1800000.00),
    ('6f9619ff-8b86-d011-b42d-00c04fc998ff', 'Brad', 'Pitt', '1963-12-18 00:00:00', 1900000.00);

-- Genre Table Examples
INSERT INTO "Genre" ("Id", "Type")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'Drama'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9a5ff', 'Comedy'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9a6ff', 'Action'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9a7ff', 'Romance'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9a8ff', 'Thriller');

-- Play Table Examples
INSERT INTO "Play" ("Id", "GenreId", "Title")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc9b4ff', '6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'Hamlet'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9b5ff', '6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'The Importance of Being Earnest'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9b6ff', '6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'Inception'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9b7ff', '6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'Titanic'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9b8ff', '6f9619ff-8b86-d011-b42d-00c04fc9a4ff', 'The Dark Knight');

INSERT INTO "Performance" ("Id", "ActorId", "TheaterId", "PlayId", "Time", "Date")
VALUES
    ('6f9619ff-8b86-d011-b42d-00c04fc9c4ff', '6f9619ff-8b86-d011-b42d-00c04fc994ff', '6f9619ff-8b86-d011-b42d-00c04fc984ff', '6f9619ff-8b86-d011-b42d-00c04fc9b4ff', '14:00:00', '2024-02-01'),
    ('6f9619ff-8b86-d011-b42d-00c04fc9c5ff', '6f9619ff-8b86-d011-b42d-00c04fc994ff', '6f9619ff-8b86-d011-b42d-00c04fc984ff', '6f9619ff-8b86-d011-b42d-00c04fc9b4ff', '16:30:00', '2024-02-02');

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