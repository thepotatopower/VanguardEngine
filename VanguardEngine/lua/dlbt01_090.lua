-- Serious Challenger, Clarissa

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.NameContains, "Earnescorrect", q.Grade, 1, q.Other, o.GradeOrLess, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Looking, q.Other, o.Shuffled
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.PlacedByRidingOn("Dignified Will, Clarissa") then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() then
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
		obj.LookAtTopOfDeck(7)
		obj.DisplayCards(2)
		obj.Search(1)
		obj.SendToBottom(2)
	elseif n == 2 then
		if obj.IsPlayerTurn() and not obj.Exists(3) then
			obj.SetAbilityPower(4, 5000)
		end
	end
	return 0
end