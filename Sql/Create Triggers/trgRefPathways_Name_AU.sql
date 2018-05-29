DROP TRIGGER IF EXISTS trgRefPathways_Name_AU;
CREATE TRIGGER trgRefPathways_Name_AU UPDATE OF Name ON RefPathways BEGIN
  DELETE FROM UrlContents WHERE Pathway = old.Id;
END