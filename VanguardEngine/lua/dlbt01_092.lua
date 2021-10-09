-- Little Lady, Helmina

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Race, r.Ghost, q.Other, o.NotThis, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
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
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(3, 5000)
	end
	return 0
end