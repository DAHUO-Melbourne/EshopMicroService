104. �޸�launchSettings.json�ļ����޸�configuration
105. basket��Ҫ��������������Դ��db��redis distributed cache
107. createһ��model folder�����������basket��model
121. ����˵һ��.net��postgrasql��ԭ��ɣ��������ݿ��õľ���appSettings���connectionString����Ϣ
	 ������θ���connectionString�����Ϣ�����������ݿ��أ�����Ҫ����connectionString�����ֵ��д��Ӧ��docker file���Ӷ�ʹ��docker-compose�������������ݿ��ָ��
126. �����docker�������⣺
	
	docker-compose -f docker-compose.yml -f docker-compose.override.yml down

	docker volume rm <your_project_name>_postgres_catalog
	docker volume rm <your_project_name>_postgres_basket

	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build -d

	docker ps

	docker logs basketdb
