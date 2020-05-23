#version 430

out vec4 FragColor;
in 	vec3 glPosition;

const float EPSILON = 0.0005;
const float BIG = 1.0 / 0.0;
const vec3  Unit = vec3 ( 1.0, 1.0, 1.0 );
	
const int MATERIAL_DIFFUSE 	  = 1;
const int MATERIAL_REFLECTION = 2;
const int MATERIAL_REFRACTION = 3;

const int DIFFUSE_REFLECTION = 1;
const int MIRROR_REFLECTION  = 2;


const int SPHERES_COUNT   = 3;
const int TRIANGLES_COUNT = 10;
const int MATERIAL_COUNT  = 6;
const int STACK_LENGTH    = 10;

const vec3 COLOR_RED    = vec3(1.0, 0.0, 0.0);
const vec3 COLOR_GREEN  = vec3(0.0, 1.0, 0.0);
const vec3 COLOR_YELLOW = vec3(1.0, 1.0, 0.0);
const vec3 COLOR_BLUE   = vec3(0.0, 0.0, 1.0);
const vec3 COLOR_WHITE  = vec3(1.0, 1.0, 1.0);


//const vec3 LIGHT_POSITION = vec3(2.0, 4.0, -4.0);

int StackPTR = -1;

struct SSphere { 
	vec3  Center;
	float Radius;
	
	int   MaterialIdx;
};

struct STriangle {
	vec3 v1;
	vec3 v2;
	vec3 v3;
	
	int  MaterialIdx;
};



struct Cam {
	vec3 Position;
	vec3 View;
	vec3 Up;
	vec3 Side;
	vec2 Scale;
};

struct Ray {
	vec3 Origin;
	vec3 Direction;
};

struct SIntersection {
	float Time;
	vec3  Point;
	vec3  Normal;
	vec3  Color;
	vec4  LightCoeffs;
	
	float ReflectionCoef;
	float RefractionCoef;
	
	int   MaterialType;
};

struct SMaterial { 
	vec3  Color;
	vec4  LightCoeffs;
	
	float ReflectionCoef;
	float RefractionCoef;
	
	int   MaterialType;
};

struct SLight {
	vec3 Position;
};

struct STracingRay {
	Ray   ray;
	float contribution;
	
	int   depth;
};

SLight 	  	light;
STriangle 	triangles [TRIANGLES_COUNT];
SSphere   	spheres	  [SPHERES_COUNT];
SMaterial 	materials [MATERIAL_COUNT];
STracingRay Stack	  [STACK_LENGTH];

uniform vec3  LIGHT_POSITION;
uniform Cam   uCamera;
const int CUBE_COUNT = 10;
const int CUBE_TRIANGLES_COUNT = 12;
const int CUBE_TOTAL_TRIANGLES_COUNT = CUBE_COUNT * CUBE_TRIANGLES_COUNT;
uniform STriangle CubeTriangles[CUBE_TOTAL_TRIANGLES_COUNT];

Ray GenerateRay() {
	vec2 crd = glPosition.xy * uCamera.Scale;
	vec3 dir = uCamera.View + uCamera.Side * crd.x + uCamera.Up * crd.y;
	
	return Ray(uCamera.Position, normalize(dir));
}

/* Cam InitCameraDefaults() {
	Cam camera;
	
	camera.Up 		= vec3(0.0, 1.0,  0.0);
	camera.View	 	= vec3(0.0, 0.0,  1.0);
	camera.Side 	= vec3(1.0, 0.0,  0.0);
	camera.Position	= vec3(0.0, 0.0, -8.0);
	
	camera.Scale 	= vec2(1.0);
	
	return camera;
} */

bool pushRay (STracingRay newray) {
	bool canPlaced = StackPTR < STACK_LENGTH;
	
	if(canPlaced) {
		StackPTR += 1;
		Stack[StackPTR] = newray;
	}
	
	return canPlaced;
} 

STracingRay popRay () {
	StackPTR -= int(StackPTR >= 0);
	return Stack[StackPTR + 1];
}

bool isEmpty() {
	return !(StackPTR >= 0);
}



void initializeDefaultLightMaterials(out SLight light, out SMaterial materials[MATERIAL_COUNT]) {

	light.Position = LIGHT_POSITION;

	vec4 lightCoefs = vec4(0.4, 0.9, 0.0, 512.0);
	
	materials[0].Color 		 	= COLOR_RED;
	materials[0].LightCoeffs 	= lightCoefs;
	materials[0].ReflectionCoef = 0.5;
	materials[0].RefractionCoef = 1.0;
	materials[0].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[1].Color 			= COLOR_GREEN;
	materials[1].LightCoeffs 	= lightCoefs;
	materials[1].ReflectionCoef = 0.5;
	materials[1].RefractionCoef = 1.0;
	materials[1].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[2].Color 			= COLOR_BLUE;
	materials[2].LightCoeffs 	= lightCoefs;
	materials[2].ReflectionCoef = 0.5;
	materials[2].RefractionCoef = 1.0;
	materials[2].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[3].Color 			= COLOR_YELLOW;
	materials[3].LightCoeffs 	= lightCoefs;
	materials[3].ReflectionCoef = 0.5;
	materials[3].RefractionCoef = 1.0;
	materials[3].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[4].Color 			= COLOR_WHITE;
	materials[4].LightCoeffs 	= lightCoefs;
	materials[4].ReflectionCoef = 0.5;
	materials[4].RefractionCoef = 1.0;
	materials[4].MaterialType 	= MATERIAL_DIFFUSE;
	
	
	materials[5].Color 			= COLOR_WHITE;
	materials[5].LightCoeffs 	= lightCoefs;
	materials[5].ReflectionCoef = 0.5;
	materials[5].RefractionCoef = 1.0;
	materials[5].MaterialType 	= MATERIAL_REFLECTION;
}

void initializeDefaultScene( out STriangle triangles[TRIANGLES_COUNT], out SSphere spheres[SPHERES_COUNT] ) {
	/** TRIANGLES **/
	
	/* left wall */
	triangles[0].v1 = vec3(-5.0,-5.0,-5.0);
	triangles[0].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[0].v3 = vec3(-5.0, 5.0,-5.0);
	triangles[0].MaterialIdx = 3;
	
	triangles[1].v1 = vec3(-5.0,-5.0,-5.0);
	triangles[1].v2 = vec3(-5.0,-5.0, 5.0);
	triangles[1].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[1].MaterialIdx = 3;
	
	/* back wall */
	triangles[2].v1 = vec3(-5.0,-5.0, 5.0);
	triangles[2].v2 = vec3( 5.0,-5.0, 5.0);
	triangles[2].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[2].MaterialIdx = 0;
	
	triangles[3].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[3].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[3].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[3].MaterialIdx = 0;
	
	/* right wall */
	triangles[4].v1 = vec3( 5.0,-5.0,-5.0);
	triangles[4].v2 = vec3( 5.0, 5.0,-5.0);
	triangles[4].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[4].MaterialIdx = 3;
	
	triangles[5].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[5].v2 = vec3( 5.0,-5.0, 5.0);
	triangles[5].v3 = vec3( 5.0, 5.0,-5.0);
	triangles[5].MaterialIdx = 3;
	
	/* bottom wall */
	triangles[6].v1 = vec3(-5.0,-5.0, 5.0);
	triangles[6].v2 = vec3(-5.0,-5.0,-5.0);
	triangles[6].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[6].MaterialIdx = 4;
	
	triangles[7].v1 = vec3( 5.0,-5.0, 5.0);
	triangles[7].v2 = vec3(-5.0,-5.0,-5.0);
	triangles[7].v3 = vec3( 5.0,-5.0,-5.0);
	triangles[7].MaterialIdx = 4;
	
	/* top wall */
	triangles[8].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[8].v2 = vec3( 5.0, 5.0,-5.0);
	triangles[8].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[8].MaterialIdx = 4;
	
	triangles[9].v1 = vec3(-5.0, 5.0,-5.0);
	triangles[9].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[9].v3 = vec3( 5.0, 5.0,-5.0);
	triangles[9].MaterialIdx = 4;
	
	/* front wall */
	//triangles[10].v1 = vec3(-5.0,-5.0, 5.0);
	//triangles[10].v2 = vec3( 5.0,-5.0, 5.0);
	//triangles[10].v3 = vec3(-5.0, 5.0, 5.0);
	//triangles[10].MaterialIdx = 0;
	
	//triangles[11].v1 = vec3( 5.0, 5.0, 5.0);
	//triangles[11].v2 = vec3(-5.0, 5.0, 5.0);
	//triangles[11].v3 = vec3( 5.0,-5.0, 5.0);
	//triangles[11].MaterialIdx = 0;
	
	/** SPHERES **/
	spheres[0].Center = vec3(-1.0,-1.0,-2.0);
	spheres[0].Radius = 0.5;
	spheres[0].MaterialIdx = 4;
	
	spheres[1].Center = vec3(2.0,1.0,2.0);
	spheres[1].Radius = 1.0;
	spheres[1].MaterialIdx = 0;
	
	
	//spheres[2].Center = LIGHT_POSITION + vec3(0.6, 0.6, -0.1);
	//spheres[2].Radius = .1;
	//spheres[2].MaterialIdx = 4;
}

bool IntersectSphere( SSphere sphere, Ray ray, float start, float final, out float time ) {
	ray.Origin -= sphere.Center;
	
	// dot - scalar multiply
	float A = dot(ray.Direction, ray.Direction);
	float B = dot(ray.Direction, ray.Origin);
	float C = dot(ray.Origin, ray.Origin) - sphere.Radius * sphere.Radius;
	float D = B * B - A * C;
	
	if (D >= 0.0) {
		D = sqrt(D);
		
		float t1 = (-B - D) / A;
		float t2 = (-B + D) / A;
		
		if(t1 >= 0 || t2 >= 0) {
			if(min(t1, t2) < 0) {
				time = max(t1, t2);
			}
			else {
				time = min(t1, t2);
			}
			
			return true;
		}
	}
	
	return false;
}

bool IntersectTriangle(Ray ray, vec3 v1, vec3 v2, vec3 v3, out float time) {
	time = -1;
	
	vec3 N = cross(v2 - v1, v3 - v1);
	
	float NdotRayDirection = dot(N, ray.Direction);
	
	if (abs(NdotRayDirection) < EPSILON) {
		return false;
	}
	
	float t = -(dot(N, ray.Origin) - dot(N, v1)) / NdotRayDirection;
	
	if (t < 0.0) {
		return false;
	}
	
	vec3 P = ray.Origin + t * ray.Direction;
	vec3 C;
	
	C = cross(v2 - v1, P - v1);
	if (dot(N, C) < 0.0) {
		return false;
	}
	
	C = cross(v3 - v2, P - v2);
	if (dot(N, C) < 0.0) {
		return false;
	}
	
	C = cross(v1 - v3, P - v3);
	if (dot(N, C) < 0.0) {
		return false;
	}
	
	time = t;
	return true;
}

bool Raytrace(Ray ray, float start, float final, inout SIntersection intersect ) {
	bool  result = false;
	float test   = start;
	
	int		  MaterialIdx;
	STriangle triangle;
	SSphere	  sphere;
	
	intersect.Time = final;
	
	for (int i = 0; i < SPHERES_COUNT; i++) {
		sphere 		= spheres[i];
		MaterialIdx = sphere.MaterialIdx;
		
		if (IntersectSphere(sphere, ray, start, final, test) && test < intersect.Time) {
			intersect.Time 	 = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(intersect.Point - spheres[i].Center);
			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs 	 = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;
			
			result = true;
		}
	}
	
	for (int i = 0; i < TRIANGLES_COUNT; i++) {
		triangle 	= triangles[i];
		MaterialIdx = triangle.MaterialIdx;
		
		if (IntersectTriangle(ray, triangle.v1, triangle.v2, triangle.v3, test) && test < intersect.Time ) {
			intersect.Time   = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(cross(triangle.v1 - triangle.v2, triangle.v3 - triangle.v2));
			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs    = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;
			
			result = true;
		}
	}
	
	for (int i = 0; i < CUBE_TOTAL_TRIANGLES_COUNT; i++) {
		triangle 	= CubeTriangles[i];
		MaterialIdx = triangle.MaterialIdx;
		
		if (IntersectTriangle(ray, triangle.v1, triangle.v2, triangle.v3, test) && test < intersect.Time ) {
			intersect.Time   = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(cross(triangle.v1 - triangle.v2, triangle.v3 - triangle.v2));
			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs    = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;
			
			result = true;
		}
	}
		
	return result;
}

float Shadow(SLight currLight, SIntersection intersect) {
	vec3  dir = normalize(currLight.Position - intersect.Point);
	float dis = distance(currLight.Position, intersect.Point);

	Ray shadowRay = Ray(intersect.Point + dir * EPSILON, dir);
	SIntersection shadowIntersect;
	shadowIntersect.Time = BIG;

	return int(!Raytrace(shadowRay, 0, dis, shadowIntersect));
}

vec3 Phong(SIntersection intersect, SLight currLight, float shadow) {
	vec3  light     = normalize(currLight.Position - intersect.Point);
	vec3  view 	    = normalize(uCamera.Position   - intersect.Point);
	float diffuse   = max(dot(light, intersect.Normal), 0.0);
	
	vec3  reflected = reflect(-view, intersect.Normal);
	
	float specular = pow(max(dot(reflected, light), 0.0), intersect.LightCoeffs.w);
	
	return intersect.LightCoeffs.x * intersect.Color +
		   intersect.LightCoeffs.y * diffuse * intersect.Color * shadow +
		   intersect.LightCoeffs.z * specular * Unit;
}

void main ( void ) {
	float start = 0;
	float final = BIG;
	vec3  resultColor = vec3(0,0,0);
	
	//uCamera = InitCameraDefaults();
	Ray ray = GenerateRay();
	
	initializeDefaultScene(triangles, spheres);
	initializeDefaultLightMaterials(light, materials);
	
	STracingRay sray = STracingRay(ray, 1, 0);
	pushRay(sray);
	
	while (!isEmpty()) {
		SIntersection intersect;
		STracingRay sray = popRay();
		intersect.Time = BIG;
		ray = sray.ray;
		
		start = 0;
		final = BIG;
		
		if (Raytrace(ray, start, final, intersect)) {
			switch (intersect.MaterialType) {
				case DIFFUSE_REFLECTION: {
				
					float shadow = Shadow(light, intersect);
					resultColor += sray.contribution * Phong(intersect, light, shadow);
					break;
					
				}
				
				case MIRROR_REFLECTION: {
				
					if (intersect.ReflectionCoef < 1) {
						float shadow = Shadow(light, intersect);
						float contribution = sray.contribution * (1 - intersect.ReflectionCoef);
						
						resultColor += contribution * Phong(intersect, light, shadow);
					}
					
					vec3 reflectDirection = reflect(ray.Direction, intersect.Normal);
					
					float contribution = sray.contribution * intersect.ReflectionCoef;
					STracingRay reflectRay = STracingRay(Ray(intersect.Point + reflectDirection * EPSILON, reflectDirection), contribution, sray.depth + 1);
					
					pushRay(reflectRay);
					break;
					
				}
			}
		}
	}
	
	FragColor = vec4 (resultColor, 1.0);
}