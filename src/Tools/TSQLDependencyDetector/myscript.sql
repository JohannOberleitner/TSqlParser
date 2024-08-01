
SELECT a,b,c FROM MyTable

CREATE PROCEDURE dbo.MyStoredProc 
(
  @fid INT NOT NULL,
  @atext VARCHAR(20)
)
AS
BEGIN
  SELECT d 
  FROM MyTable 
  WHERE 
    id = fid

END
