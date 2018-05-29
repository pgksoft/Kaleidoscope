DROP TABLE IF EXISTS UrlContents;
CREATE TABLE UrlContents
(
	'Id'         INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	'Name'       TEXT NOT NULL,
    'Pathway'    INTEGER NOT NULL,
	'ImageType'  INTEGER,
	'VideoType'  INTEGER, 
	'RefComment' INTEGER,
	Description  TEXT,
	CONSTRAINT 'UrlContentPathway_Fk' FOREIGN KEY (Pathway) REFERENCES RefPathways (Id) ON DELETE CASCADE,
	CONSTRAINT 'UrlContentImageType_Fk' FOREIGN KEY (ImageType) REFERENCES RefImageTypes (Id),
	CONSTRAINT 'UrlContentVideoType_Fk' FOREIGN KEY (VideoType) REFERENCES RefVideoTypes (Id),
	CONSTRAINT 'UrlContentComment_Fk' FOREIGN KEY (RefComment) REFERENCES RefComments (Id),
	CONSTRAINT 'UrlContent_Type_Ch' CHECK(ImageType IS NOT NULL AND VideoType IS NULL OR ImageType IS NULL AND VideoType IS NOT NULL)
);