-- Sunlight Punishment

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Min, 1
	elseif n == 2 then 
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.SpecificCB, 1
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
		if obj.CanRetire(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Inject(3, q.Count, obj.CBUsed())
		obj.ChooseRetire(3)
	end
	return 0
end