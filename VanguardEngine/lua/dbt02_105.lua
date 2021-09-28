-- Black Sage, Charon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.NameContains, "Blaster", q.Count, 1
	elseif n == 3 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 1
	elseif n == 4 then
		return q.Location, l.LastCalled
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.Rest, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.Exists(2) then
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
		obj.LookAtTopOfDeck(1)
		if obj.Exists(3) and obj.SuperiorCall(3) then
			obj.RetireAtEndOfTurn(4)
		end
	end
	return 0
end