-- SQL Script for ComicBook Database
-- Created: July 18, 2025

-- Create Tables
CREATE TABLE Categories (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(200)
);

CREATE TABLE ComicBooks (
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    Author VARCHAR(100) NOT NULL,
    Artist VARCHAR(100) NOT NULL,
    PublicationDate DATE,
    PageCount INT,
    Price NUMERIC(10, 2) NOT NULL,
    IsAvailable BOOLEAN NOT NULL DEFAULT TRUE,
    CategoryId INT NOT NULL REFERENCES Categories(Id),
    CreatedDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Insert Seed Data
-- Categories
INSERT INTO Categories (Name, Description)
VALUES 
    ('Superhero', 'Comic books featuring superheroes and their adventures'),
    ('Manga', 'Japanese comic books and graphic novels'),
    ('Science Fiction', 'Comic books with science fiction themes'),
    ('Fantasy', 'Comic books with fantasy elements and worlds'),
    ('Horror', 'Comic books with horror and supernatural themes'),
    ('Action/Adventure', 'Comic books with action-packed adventures'),
    ('Mystery', 'Comic books featuring detective work and mysteries'),
    ('Historical', 'Comic books set in historical time periods'),
    ('Slice of Life', 'Comic books depicting everyday experiences'),
    ('Comedy', 'Comic books with humorous content');

-- Comic Books
INSERT INTO ComicBooks (Title, Author, Artist, Description, PublicationDate, PageCount, Price, IsAvailable, CategoryId, CreatedDate)
VALUES 
    ('Batman: Year One', 'Frank Miller', 'David Mazzucchelli', 'Batman''s early days fighting crime in Gotham City', '1987-02-01', 96, 19.99, TRUE, 1, CURRENT_TIMESTAMP),
    ('One Piece', 'Eiichiro Oda', 'Eiichiro Oda', 'Monkey D. Luffy and his pirate crew search for the greatest treasure, the One Piece', '1997-07-22', 208, 9.99, TRUE, 2, CURRENT_TIMESTAMP),
    ('Saga', 'Brian K. Vaughan', 'Fiona Staples', 'Epic space opera/fantasy comic book series', '2012-03-14', 44, 14.99, TRUE, 3, CURRENT_TIMESTAMP),
    ('The Sandman', 'Neil Gaiman', 'Sam Kieth', 'The Lord of Dreams has been imprisoned for decades and must reclaim his power', '1989-01-01', 40, 24.99, TRUE, 4, CURRENT_TIMESTAMP),
    ('Uzumaki', 'Junji Ito', 'Junji Ito', 'A town is haunted by a supernatural spiral pattern', '1998-01-01', 200, 22.99, TRUE, 5, CURRENT_TIMESTAMP),
    ('Superman: Red Son', 'Mark Millar', 'Dave Johnson', 'What if Superman had been raised in the Soviet Union?', '2003-04-01', 48, 17.99, TRUE, 1, CURRENT_TIMESTAMP),
    ('Naruto', 'Masashi Kishimoto', 'Masashi Kishimoto', 'Young ninja Naruto Uzumaki seeks recognition and dreams of becoming the Hokage', '1999-09-21', 192, 10.99, TRUE, 2, CURRENT_TIMESTAMP),
    ('Watchmen', 'Alan Moore', 'Dave Gibbons', 'An alternate history where superheroes emerged in the 1940s and 1960s', '1986-09-01', 416, 29.99, TRUE, 1, CURRENT_TIMESTAMP),
    ('Attack on Titan', 'Hajime Isayama', 'Hajime Isayama', 'Humanity fights for survival against man-eating giants', '2009-09-09', 192, 12.99, TRUE, 2, CURRENT_TIMESTAMP),
    ('Dune: House Atreides', 'Brian Herbert', 'Dev Pramanik', 'Adaptation of the sci-fi novel set in the Dune universe', '2020-10-21', 32, 16.99, TRUE, 3, CURRENT_TIMESTAMP),
    ('Maus', 'Art Spiegelman', 'Art Spiegelman', 'A survivor''s tale of the Holocaust with Jews depicted as mice and Nazis as cats', '1980-01-01', 296, 15.99, TRUE, 8, CURRENT_TIMESTAMP),
    ('Persepolis', 'Marjane Satrapi', 'Marjane Satrapi', 'Autobiographical comic about growing up in Iran during the Islamic Revolution', '2000-01-01', 160, 14.99, TRUE, 8, CURRENT_TIMESTAMP),
    ('Scott Pilgrim', 'Bryan Lee O''Malley', 'Bryan Lee O''Malley', 'Scott must defeat his new girlfriend''s seven evil exes', '2004-08-01', 168, 11.99, TRUE, 9, CURRENT_TIMESTAMP),
    ('Bone', 'Jeff Smith', 'Jeff Smith', 'Three cousins get lost in a mysterious valley filled with wonderful and terrifying creatures', '1991-07-01', 140, 12.99, TRUE, 4, CURRENT_TIMESTAMP),
    ('Hellboy', 'Mike Mignola', 'Mike Mignola', 'A demon summoned by Nazis works as a paranormal investigator', '1993-03-01', 128, 18.99, TRUE, 5, CURRENT_TIMESTAMP),
    ('Y: The Last Man', 'Brian K. Vaughan', 'Pia Guerra', 'A mysterious plague kills all male mammals except one man and his monkey', '2002-09-01', 60, 15.99, TRUE, 3, CURRENT_TIMESTAMP),
    ('Asterix', 'René Goscinny', 'Albert Uderzo', 'Adventures of Gaulish warriors resisting Roman occupation in 50 BCE', '1959-10-29', 48, 13.99, TRUE, 6, CURRENT_TIMESTAMP),
    ('Tintin', 'Hergé', 'Hergé', 'Adventures of young Belgian reporter and his dog Snowy', '1929-01-10', 62, 14.99, TRUE, 6, CURRENT_TIMESTAMP),
    ('Blacksad', 'Juan Díaz Canales', 'Juanjo Guarnido', 'Anthropomorphic animals in a noir detective setting', '2000-11-01', 56, 19.99, TRUE, 7, CURRENT_TIMESTAMP),
    ('Calvin and Hobbes', 'Bill Watterson', 'Bill Watterson', 'Adventures of a young boy and his imaginary tiger friend', '1985-11-18', 128, 16.99, TRUE, 10, CURRENT_TIMESTAMP);


-- Create Indexes
CREATE INDEX IX_ComicBooks_CategoryId ON ComicBooks(CategoryId);
CREATE INDEX IX_ComicBooks_Title ON ComicBooks(Title);
CREATE INDEX IX_ComicBooks_Author ON ComicBooks(Author);

-- Create View
CREATE VIEW vw_ComicBooksWithCategories AS
SELECT 
    cb.Id,
    cb.Title,
    cb.Description,
    cb.Author,
    cb.Artist,
    cb.PublicationDate,
    cb.PageCount,
    cb.Price,
    cb.IsAvailable,
    cb.CreatedDate,
    c.Id AS CategoryId,
    c.Name AS CategoryName,
    c.Description AS CategoryDescription
FROM 
    ComicBooks cb
JOIN 
    Categories c ON cb.CategoryId = c.Id;

-- Create Function (Stored Procedure alternative in PostgreSQL)
CREATE OR REPLACE FUNCTION sp_search_comicbooks(
    search_term VARCHAR DEFAULT NULL,
    category_id INT DEFAULT NULL,
    min_price NUMERIC DEFAULT NULL,
    max_price NUMERIC DEFAULT NULL
)
RETURNS TABLE (
    Id INT,
    Title VARCHAR,
    Description VARCHAR,
    Author VARCHAR,
    Artist VARCHAR,
    Price NUMERIC,
    CategoryName VARCHAR
)
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        cb.Id,
        cb.Title,
        cb.Description,
        cb.Author,
        cb.Artist,
        cb.Price,
        c.Name AS CategoryName
    FROM ComicBooks cb
    JOIN Categories c ON cb.CategoryId = c.Id
    WHERE 
        (search_term IS NULL OR
         cb.Title ILIKE '%' || search_term || '%' OR
         cb.Author ILIKE '%' || search_term || '%' OR
         cb.Artist ILIKE '%' || search_term || '%' OR
         cb.Description ILIKE '%' || search_term || '%')
        AND (category_id IS NULL OR cb.CategoryId = category_id)
        AND (min_price IS NULL OR cb.Price >= min_price)
        AND (max_price IS NULL OR cb.Price <= max_price)
    ORDER BY cb.Title;
END;
$$ LANGUAGE plpgsql;
