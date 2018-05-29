DROP VIEW IF EXISTS vCatalogs;
CREATE VIEW VCatalogs AS 
SELECT 
  c.Id                                                    as Id, 
  c.Node                                                  as Node, 
  rnode.Name                                              as SNode, 
  c.ParentID                                              as ParentID, 
  (select r.Name from RefNodes as r where r.Id = c1.Node) as SParent 
FROM Catalogs as c 
  JOIN RefNodes as rnode on rnode.Id = c.Node 
  LEFT JOIN Catalogs as c1 on c1.Id = c.ParentID
