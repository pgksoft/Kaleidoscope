DROP VIEW IF EXISTS vTreeCatalogs;
CREATE VIEW vTreeCatalogs AS
WITH RECURSIVE temp(Id, Node, SNode, ParentID, SParent, Level) AS (
SELECT
  Id,
  Node,
  SNode,
  ParentID,
  SParent,
  0 as Level
FROM VCatalogs
WHERE ParentID IS NULL
UNION ALL 
SELECT 
  v.Id, 
  v.Node, 
  v.SNode, 
  v.ParentID, 
  v.SParent,
  Level + 1
FROM VCatalogs as v 
  JOIN temp as s on v.ParentID = s.Id 
ORDER BY Level DESC
)
SELECT * FROM temp 