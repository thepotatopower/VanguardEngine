-- Munching Girl, Seeya

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
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
		obj.DisplayCards(1)
		if not (obj.CanSuperiorCallToSpecificCircle(1, FL.OpenCircle) and obj.SelectOption("Call to open RC", "Add to hand and discard 1") == 1 and obj.SuperiorCallToSpecificCircle(1, FL.OpenCircle)) then
			obj.AddToHand(1)
			obj.Discard(1)
		end
	end
	return 0
end