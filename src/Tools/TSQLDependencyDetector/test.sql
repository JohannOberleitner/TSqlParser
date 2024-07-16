
SELECT myfunc(abc) FROM dbo.mytable1 m1 inner join dbo.mytable2 m2 on m1.id=m2.id
         LEFT OUTER JOIN mytable3 m3 on m3.id = myfunc(m1.id)

CREATE PROCEDURE dbo.myStoredProc(
    @parameter1 INT) 
AS
BEGIN
  SELECT * FROM dbo.mytable2 WHERE id = @parameter1
  
  INSERT INTO dbo.mytable1(id, data_item) VALUES(@parameter1, 'xyz')

END

UPDATE tableX SET a = 1

EXEC dbo.myStoredProc 3

SELECT * FROM tableX

insert into dbo.table4(a,b) 	             
select x,y from dbo.table5



