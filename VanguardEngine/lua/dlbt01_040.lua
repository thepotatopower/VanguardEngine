-- Mystic Voice, Renata

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Drop, q.Other, o.Gem, q.Other, o.SameName, q.Count, 2, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Drop, q.Other, o.Gem, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Selected, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Drop, q.Other, o.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.Exists(3) then
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
		obj.Select(2)
		obj.ChooseSendToBottom(4)
		obj.AddToSoul(5)
	end
	return 0
end