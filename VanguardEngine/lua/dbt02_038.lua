-- Cardinal Noid, Plasteia

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Shadow Army", q.UnitType, u.Token, q.Other, o.Boosting, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(1) then
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
		obj.Draw(1)
	end
	return 0
end