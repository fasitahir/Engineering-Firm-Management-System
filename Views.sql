  ALTER VIEW ViewApplicants AS
  SELECT *
  FROM Person p 
  JOIN Applicant a ON p.PersonID = a.ApplicantID
  JOIN Lookup L ON a.DesiredDesignation = L.lookupID




  ALTER VIEW ViewEmployee AS
  SELECT *
  FROM Employee E
  JOIN Person P ON P.PersonID = E.EmployeeID
  JOIN Lookup l ON E.DesignationID = l.lookupID


 


                                      