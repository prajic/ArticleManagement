INSERT INTO Users (Email, PasswordHash, DisplayName, ImageUrl, IsVerified) VALUES
('user1@example.com', '$2a$11$QKZ8w5cM83I4F2w.ZoTDK.3D2e8OTJDatjeLSAlYwKdo355otIbGy', 'User One', NULL, 1),
('user2@example.com', '$2a$11$QKZ8w5cM83I4F2w.ZoTDK.3D2e8OTJDatjeLSAlYwKdo355otIbGy', 'User Two', NULL, 1),
('user3@example.com', '$2a$11$QKZ8w5cM83I4F2w.ZoTDK.3D2e8OTJDatjeLSAlYwKdo355otIbGy', 'User Three', NULL, 1),
('user4@example.com', '$2a$11$QKZ8w5cM83I4F2w.ZoTDK.3D2e8OTJDatjeLSAlYwKdo355otIbGy', 'User Four', NULL, 1),
('user5@example.com', '$2a$11$QKZ8w5cM83I4F2w.ZoTDK.3D2e8OTJDatjeLSAlYwKdo355otIbGy', 'User Five', NULL, 1);

INSERT INTO Authors (UserId, FirstName, LastName, Biography, SocialMediaLinks, ImageUrl, Website, PublishedArticlesCount) VALUES
(1, 'Author', 'One', 'Author One Biography', 'twitter.com/author1', 'https://example.com/author1.jpg', 'https://author1.com', 10),
(3, 'Author', 'Three', 'Author Three Biography', 'twitter.com/author3', 'https://example.com/author3.jpg', 'https://author3.com', 5),
(5, 'Author', 'Five', 'Author Five Biography', 'twitter.com/author5', 'https://example.com/author5.jpg', 'https://author5.com', 8);

INSERT INTO Articles (Title, Subtitle, Content, PublishDate, AuthorId, ReadCount, ArticleType) VALUES
('First Article', 'First Subtitle', 'This is the content of the first article', GETDATE(), 1, 100, 1),
('Second Article', 'Second Subtitle', 'This is the content of the second article', GETDATE(), 3, 50, 2),
('Third Article', 'Third Subtitle', 'This is the content of the third article', GETDATE(), 5, 30, 1);

INSERT INTO ArticleOwners (ArticleId, UserId) VALUES
(1, 1), 
(2, 3), 
(3, 5); 

INSERT INTO VerificationCodes (UserId, Code, ExpiresAt) VALUES
(1, '123456', DATEADD(DAY, 1, GETDATE())),
(2, '654321', DATEADD(DAY, 1, GETDATE()));

INSERT INTO ArticleLikes (ArticleId, UserId) VALUES
(1, 2), 
(1, 3), 
(2, 1), 
(3, 4); 

INSERT INTO ArticleComments (ArticleId, UserId, Content) VALUES
(1, 2, 'Great article!'), 
(2, 1, 'Very informative'), 
(3, 4, 'Loved this!'); 

INSERT INTO Notifications (UserId, NotificationType, Link) VALUES
(1, 'Like', '/articles/1'), 
(2, 'Comment', '/articles/2/comments');
