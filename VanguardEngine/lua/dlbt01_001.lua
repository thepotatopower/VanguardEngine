-- Earnescorrect Leader, Clarissa

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Name, "Aim to be the Strongest Idol!", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.NameContains, "Earnescorrect", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.CB, 1
	elseif n == 2 then
		return a.OnAttackHits, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.PlacedByRidingOn("Accurate Interval, Clarissa") then 
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Search(1)
		obj.Shuffle()
	elseif n == 2 then
		obj.Draw(1)
		obj.ChooseAddTempPower(2, 5000)
	end
	return 0
end