from flask import Flask, request, jsonify
import tempfile
import os
import subprocess

app = Flask(__name__)

@app.route('/run', methods=['POST'])
def run_command():
    # 获取请求参数
    x = request.form.get('x')
    y = request.form.get('y')
    
    if x is None or y is None:
        return jsonify({"error": "Missing parameters x or y"}), 400
    
    # 创建临时目录
    with tempfile.TemporaryDirectory() as temp_dir:
        # 文件路径
        domain_file_path = os.path.join(temp_dir, 'Domain.pddl')
        problem_file_path = os.path.join(temp_dir, 'Problem.pddl')
        
        # 保存参数到文件
        with open(domain_file_path, 'w') as domain_file:
            domain_file.write(x)
        
        with open(problem_file_path, 'w') as problem_file:
            problem_file.write(y)
        
        # 执行 shell 脚本并传递临时目录路径作为参数
        result = subprocess.run(
            ["/bin/bash", "test.sh", temp_dir],
            capture_output=True,
            text=True
        )
        
        # 返回脚本的输出作为 HTTP 响应
        return jsonify({
            "stdout": result.stdout,
            "stderr": result.stderr,
            "returncode": result.returncode
        })

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80, debug=True)