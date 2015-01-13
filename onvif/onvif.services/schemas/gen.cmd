svcutil.exe /t:code *.wsdl *.xsd 
	/n:http://mydomain.com/xsd/Model/Shared/2009/07/01,MyDomain.Model.Shared 
	/n:http://mydomain.com/xsd/Model/Customer/2009/07/01,MyDomain.Model.Customer
	/n:http://mydomain.com/wsdl/CustomerService-v1.0,MyDomain.CustomerServiceProxy 
	/n:http://mydomain.com/xsd/Model/Store/2009/07/01,MyDomain.Model.Store 
	/n:http://mydomain.com/wsdl/StoreService-v1.0,MyDomain.StoreServiceProxy
	/o:TestClient\WebServiceProxy3.cs
pause