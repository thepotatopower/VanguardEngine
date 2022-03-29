-- Exquisite Knight, Olwein

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Selected
	elseif n == 3 then
		return q.Location, l.Looking
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.SB, 1
	elseif n == 2 then
		return a.OnDriveCheck, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Hexaorb Sorceress") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and obj.TriggerRevealed() then
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
		obj.LookAtTopOfDeck(2)
		obj.ChooseSendToTop(1)
		obj.RearrangeOnBottom(3)
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(4, 10000)
	end
	return 0
end