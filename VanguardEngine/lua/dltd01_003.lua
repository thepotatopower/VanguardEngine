-- Astesice, Nanami

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 1, q.Other, o.GradeOrLess, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt
	elseif n == 2 then
		return a.PlacedOnRC, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.CanSuperiorCall(1) then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCallAsRest(1)
	elseif n == 2 then
		obj.AddTempPower(2, 2000)
	end
	return 0
end