-- Cardinal Prima, Ecolpa

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Shadow Army", q.UnitType, u.Token, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Other, o.FaceDown, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and (obj.IsAttackingUnit() or obj.IsBooster()) and obj.IsAbyssalDarkNight() then
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

function Activate(n)
	if n == 1 then
		obj.CounterCharge(1)
	end
	return 0
end