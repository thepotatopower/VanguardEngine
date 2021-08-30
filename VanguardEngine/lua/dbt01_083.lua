-- Grapple External

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerPrisoners, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.IsMandatory
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
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.Exists(2) and obj.IsAttackingUnit() then
			obj.SetAbilityPower(1, 5000)
		end
	end
	return 0
end