-- Heavenly Staff of Kind Intention, Cortese

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.RevealedTrigger, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.OpenCirclesExist(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1, FL.OpenCircle)
		obj.AddSkill(2, s.Boost)
	end
	return 0
end