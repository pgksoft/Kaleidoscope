DROP VIEW IF EXISTS VUrlContents;
CREATE VIEW VUrlContents AS 
SELECT 
  u.Id              as Id, 
  u.Name            as Name, 
  u.Pathway         as NPathway, 
  pth.Name          as SPathway,
  u.ImageType       as NImageType, 
  itp.Name          as SImageType,
  u.VideoType       as NVideoType,
  vtp.Name          as SVideoType,
  u.RefComment      as NRefComment,
  cmnt.Name         as SRefComment,
  u.Description     as Description, 
  prop.Id           as NProperty, 
  prop.Rotate       as NRotate 
FROM UrlContents as u 
  JOIN RefPathways as pth on pth.Id = u.Pathway 
  LEFT JOIN RefImageTypes as itp on itp.Id = u.ImageType
  LEFT JOIN RefVideoTypes as vtp on vtp.Id = u.VideoType
  LEFT JOIN RefComments as cmnt on cmnt.Id = u.RefComment
  LEFT JOIN ContentProperties as prop on prop.Content = u.ID