-- Nectar of Sensationalism

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop
	elseif n == 3 then
		return q.Location, l.Drop, q.Count, 15
	elseif n == 4 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Select(1)
		local count = obj.GetNumberOf(2)
		count = count - (count % 5)
		count = count / 5
		obj.AddTempPower(4, count * 5000)
		if obj.Exists(3) then
			obj.AddCritical(4, 1)
		end
		obj.EndSelect()
	end
	return 0
end