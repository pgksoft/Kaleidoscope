DROP VIEW IF EXISTS VRefPathways;
CREATE VIEW VRefPathways AS 
SELECT 
  pth.ID                             as ID,
  pth.Name                           as Name,
  count(urlc.ImageType)              as NCountImageType,
  count(urlc.VideoType)              as NCountVideoType
  --(
  -- SELECT count(*) 
  -- FROM UrlContents as urlc 
  -- WHERE urlc.Pathway = pth.ID 
  --   and urlc.ImageType IS NOT NULL
  --)                                  as NCountImageType,
  --(
  -- SELECT count(*) 
  -- FROM UrlContents as urlc 
  -- WHERE urlc.Pathway = pth.ID 
  --   and urlc.VideoType IS NOT NULL
  --)                                  as NCountVideoType

FROM RefPathways as pth
  LEFT JOIN UrlContents as urlc ON urlc.Pathway = pth.ID
GROUP BY pth.ID, pth.Name  