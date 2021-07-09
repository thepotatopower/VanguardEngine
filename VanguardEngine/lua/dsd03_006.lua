-- Knight of Broadaxe, Rafluke

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Grade, 3, q.Count, 2
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Grade, 3, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.AutoAddToSoul(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(3, 10000)
	end
	return 0
end