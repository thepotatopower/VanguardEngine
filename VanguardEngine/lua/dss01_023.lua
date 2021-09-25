-- Aurora Battle Princess, Spark Limone

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyDrop, q.Other, o.Unit, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.EnemyDrop, q.Other, o.Unit, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.HasPrison() and obj.Exists(2) then
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
		obj.ChooseImprison(1)
	end
	return 0
end
