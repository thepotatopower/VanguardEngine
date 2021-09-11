-- Frenzied Heiress

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.GC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsGuardian() then
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
		if obj.EnemyVanguardGrade() >= 3 then
			obj.SetAbilityShield(1, 5000)
		end
	end
	return 0
end