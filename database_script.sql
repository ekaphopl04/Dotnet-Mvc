-- SQL Script for ComicBook Database
-- Created: July 18, 2025

-- Create Database
CREATE DATABASE ComicBookDB;
GO

USE ComicBookDB;
GO

-- Create Tables
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NULL
);
GO

CREATE TABLE ComicBooks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    Author NVARCHAR(100) NOT NULL,
    Artist NVARCHAR(100) NOT NULL,
    PublicationDate DATETIME NULL,
    PageCount INT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CategoryId INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ComicBooks_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

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
GO

-- Comic Books
INSERT INTO ComicBooks (Title, Author, Artist, Description, PublicationDate, PageCount, Price, IsAvailable, CategoryId, CreatedDate)
VALUES 
    ('Batman: Year One', 'Frank Miller', 'David Mazzucchelli', 'Batman''s early days fighting crime in Gotham City', '1987-02-01', 96, 19.99, 1, 1, GETDATE()),
    ('One Piece', 'Eiichiro Oda', 'Eiichiro Oda', 'Monkey D. Luffy and his pirate crew search for the greatest treasure, the One Piece', 1, '1997-07-22', 208, 9.99, 1, 2, GETDATE()),
    ('Saga', 'Brian K. Vaughan', 'Fiona Staples', 'Epic space opera/fantasy comic book series', '2012-03-14', 44, 14.99, 1, 3, GETDATE()),
    ('The Sandman', 'Neil Gaiman', 'Sam Kieth', 'The Lord of Dreams has been imprisoned for decades and must reclaim his power', 1, '1989-01-01', 40, 24.99, 1, 4, GETDATE()),
    ('Uzumaki', 'Junji Ito', 'Junji Ito', 'A town is haunted by a supernatural spiral pattern', '1998-01-01', 200, 22.99, 1, 5, GETDATE()),
    ('Superman: Red Son', 'Mark Millar', 'Dave Johnson', 'What if Superman had been raised in the Soviet Union?', '2003-04-01', 48, 17.99, 1, 1, GETDATE()),
    ('Naruto', 'Masashi Kishimoto', 'Masashi Kishimoto', 'Young ninja Naruto Uzumaki seeks recognition and dreams of becoming the Hokage', '1999-09-21', 192, 10.99, 1, 2, GETDATE()),
    ('Watchmen', 'Alan Moore', 'Dave Gibbons', 'An alternate history where superheroes emerged in the 1940s and 1960s', '1986-09-01', 416, 29.99, 1, 1, GETDATE()),
    ('Attack on Titan', 'Hajime Isayama', 'Hajime Isayama', 'Humanity fights for survival against man-eating giants', '2009-09-09', 192, 12.99, 1, 2, GETDATE()),
    ('Dune: House Atreides', 'Brian Herbert', 'Dev Pramanik', 'Adaptation of the sci-fi novel set in the Dune universe', '2020-10-21', 32, 16.99, 1, 3, GETDATE()),
    ('Maus', 'Art Spiegelman', 'Art Spiegelman', 'A survivor''s tale of the Holocaust with Jews depicted as mice and Nazis as cats', '1980-01-01', 296, 15.99, 1, 8, GETDATE()),
    ('Persepolis', 'Marjane Satrapi', 'Marjane Satrapi', 'Autobiographical comic about growing up in Iran during the Islamic Revolution', '2000-01-01', 160, 14.99, 1, 8, GETDATE()),
    ('Scott Pilgrim', 'Bryan Lee O''Malley', 'Bryan Lee O''Malley', 'Scott must defeat his new girlfriend''s seven evil exes', '2004-08-01', 168, 11.99, 1, 9, GETDATE()),
    ('Bone', 'Jeff Smith', 'Jeff Smith', 'Three cousins get lost in a mysterious valley filled with wonderful and terrifying creatures', '1991-07-01', 140, 12.99, 1, 4, GETDATE()),
    ('Hellboy', 'Mike Mignola', 'Mike Mignola', 'A demon summoned by Nazis works as a paranormal investigator', '1993-03-01', 128, 18.99, 1, 5, GETDATE()),
    ('Y: The Last Man', 'Brian K. Vaughan', 'Pia Guerra', 'A mysterious plague kills all male mammals except one man and his monkey', '2002-09-01', 60, 15.99, 1, 3, GETDATE()),
    ('Asterix', 'René Goscinny', 'Albert Uderzo', 'Adventures of Gaulish warriors resisting Roman occupation in 50 BCE', '1959-10-29', 48, 13.99, 1, 6, GETDATE()),
    ('Tintin', 'Hergé', 'Hergé', 'Adventures of young Belgian reporter and his dog Snowy', '1929-01-10', 62, 14.99, 1, 6, GETDATE()),
    ('Blacksad', 'Juan Díaz Canales', 'Juanjo Guarnido', 'Anthropomorphic animals in a noir detective setting', '2000-11-01', 56, 19.99, 1, 7, GETDATE()),
    ('Calvin and Hobbes', 'Bill Watterson', 'Bill Watterson', 'Adventures of a young boy and his imaginary tiger friend', '1985-11-18', 128, 16.99, 1, 10, GETDATE());
GO

-- Create Indexes for better performance
CREATE INDEX IX_ComicBooks_CategoryId ON ComicBooks(CategoryId);
CREATE INDEX IX_ComicBooks_Title ON ComicBooks(Title);
CREATE INDEX IX_ComicBooks_Author ON ComicBooks(Author);
GO

-- Create a view for ComicBooks with Category information
CREATE VIEW vw_ComicBooksWithCategories
AS
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
INNER JOIN 
    Categories c ON cb.CategoryId = c.Id;
GO

-- Create a stored procedure to search comic books
CREATE PROCEDURE sp_SearchComicBooks
    @SearchTerm NVARCHAR(100) = NULL,
    @CategoryId INT = NULL,
    @MinPrice DECIMAL(10, 2) = NULL,
    @MaxPrice DECIMAL(10, 2) = NULL
AS
BEGIN
    SELECT 
        cb.Id,
        cb.Title,
        cb.Description,
        cb.Author,
        cb.Artist,
        cb.Price,
        c.Name AS CategoryName
    FROM 
        ComicBooks cb
    INNER JOIN 
        Categories c ON cb.CategoryId = c.Id
    WHERE 
        (@SearchTerm IS NULL OR 
         cb.Title LIKE '%' + @SearchTerm + '%' OR 
         cb.Author LIKE '%' + @SearchTerm + '%' OR 
         cb.Artist LIKE '%' + @SearchTerm + '%' OR 
         cb.Description LIKE '%' + @SearchTerm + '%')
        AND (@CategoryId IS NULL OR cb.CategoryId = @CategoryId)
        AND (@MinPrice IS NULL OR cb.Price >= @MinPrice)
        AND (@MaxPrice IS NULL OR cb.Price <= @MaxPrice)
    ORDER BY 
        cb.Title;
END;
GO
