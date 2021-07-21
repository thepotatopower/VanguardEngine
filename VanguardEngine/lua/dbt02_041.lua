-- Whimsical Machine Beast, Bugmotor

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 1, q.Other, o.OrLess
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Standing, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyDrop, q.Other, o.Unit, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.HasPrison() and obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.Rest(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseImprison(3)
	end
	return 0
end