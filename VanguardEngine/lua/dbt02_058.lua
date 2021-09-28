-- Dragritter, Nasir

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Count, 3
	elseif n == 2 then
		return q.Location, l.GC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsIntercepting() and not obj.Exists(1) then
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
		obj.AddTempShield(2, 10000)
	end
	return 0
end